"use strict";

import express from "express";
import mysql from "mysql2/promise";
import dotenv from "dotenv/config"

const app = express();
const port = process.env.PORT;  // This is taken from the .env file.

app.use(express.json());

async function connectToDB() {
  return await mysql.createConnection({
    // This is also taken from the .env file.
    host: process.env.DB_HOST,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_DATABASE,
  });
}

app.listen(port, () => {
  console.log(`Server running on http://localhost:${port}/`);
});

app.get("/", async (req, res)=>{
  res.status(200).send("ADAD")
});
  
app.get("/cards", async (request, response) => {
  let connection = null;

  try {
    connection = await connectToDB();
    const [results, fields] = await connection.execute("SELECT * FROM card");

    console.log(`${results.length} rows returned`);

    const result = {cards: results};
    console.log(result);
    response.status(200).json(result);
  }
  catch (error) {
    response.status(500);
    response.json(error);
    console.log(error);
  }
  finally {
    if (connection !== null) {
      connection.end();
      console.log("Connection closed succesfully!");
    }
  }
});

app.get("/cards/unit", async (request, response) => {
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute("SELECT * FROM unitCards");

        console.log(`${results.length} rows returned`);
        const result = {cards: results};
        console.log(result);
        response.status(200).json(result);
    }
    catch (error) {
        console.log(error);
        response.status(500).json(error);
    }
    finally {
        if (connection !== null) {
            connection.end();
            console.log("Connection closed succesfully");
        }
    }
});

app.get("/cards/paradox", async (request, response) => {
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute("SELECT * FROM paradoxCards");

        console.log(`${results.length} rows returned`);
        const result = {cards: results};
        console.log(result);
        response.status(200).json(result);
    }
    catch (error) {
        console.log(error);
        response.status(500).json(error);
    }
    finally {
        if (connection !== null) {
            connection.end();
            console.log("Connection closed succesfully");
        }
    }
});

app.post("/users/signup", async (request, response) => {
  let connection = null;

  try {
      connection = await connectToDB();
      const { username, password } = request.body;

      if (!username || !password) {
          return response.status(400).json({ error: 'Username and password are required' });
      }

      await connection.query("CALL SignUpUser(?, ?)", [username, password]);

      response.status(201).json({ message: 'User created' });
  } catch (error) {
      console.log(error);
      // Manejar errores específicos del procedimiento almacenado
      if (error.code === 'ER_SIGNAL_EXCEPTION') {
          response.status(409).json({ error: 'Username already exists' });
      } else {
          response.status(500).json({ error: 'Internal Server Error' });
      }
  } finally {
      if (connection !== null) {
          await connection.end();
          console.log("Connection closed successfully");
      }
  }
});

app.post("/users/signin", async (request, response) => {
  let connection = null;

  try {
      connection = await connectToDB();
      const { username, password } = request.body;

      if (!username || !password) {
          return response.status(400).json({ error: 'Username and password are required' });
      }

      // Llamar al procedimiento almacenado de Sign In en DB Chronicle_doom
      await connection.query("CALL SignInUser(?, ?)", [username, password]);

      response.status(200).json({ message: 'Sign in successful' });
  } catch (error) {
      console.log(error);
      // Manejar errores específicos del procedimiento almacenado
      if (error.code === 'ER_SIGNAL_EXCEPTION') {
          response.status(401).json({ error: 'Invalid username or password' });
      } else {
          response.status(500).json({ error: 'Internal Server Error' });
      }
  } finally {
      if (connection !== null) {
          await connection.end();
          console.log("Connection closed successfully");
      }
  }
});

app.post("/updatedeck", async (request, response) => {
  let connection = null;

  try {
      connection = await connectToDB();
      const { username, deck_name, card_JSON } = request.body;
      let cardNumber = 0;
        if (!card_JSON || !Array.isArray(card_JSON) || card_JSON.length === 0) {
        return response.status(400).json({ error: 'Card JSON is missing or empty' });
        }
    
      for (let i = 0; i < card_JSON.length; i++) {
          cardNumber += card_JSON[i].card_times;
      }
      if (!username || !deck_name || !card_JSON || cardNumber !== 18) {
          console.log(cardNumber);
          return response.status(400).json({ error: 'Exactly 18 cards are required to update a deck'});
      }
      
      // Llamar al procedimiento almacenado de Delete Deck en DB Chronicle_doom solo si hay cartas
      await connection.execute("CALL DeleteDeck(?)", [username]);
        console.log(card_JSON);
        console.log(card_JSON.length);
      for (let i = 0; i < card_JSON.length; i++) {
            console.log(card_JSON[i].cardID);
            console.log(card_JSON[i].card_times);
          if (card_JSON[i].cardID) {
            // Llamar al procedimiento almacenado de Update Deck en DB Chronicle_doom
            await connection.query("CALL UpdateDeck(?, ?, ?, ?)", [username, deck_name, card_JSON[i].cardID, card_JSON[i].card_times]);
          }
        }

      response.status(200).json({ message: 'Deck updated' });
  } catch (error) {
      console.log(error);
      if (error.code === 'ER_SIGNAL_EXCEPTION') {
          response.status(400).json({ error: error.sqlMessage });
      } else {
          response.status(500).json({ error: 'Internal Server Error' });
      }
  } finally {
      if (connection !== null) {
          await connection.end();
          console.log("Connection closed successfully");
      }
  }
});

app.get("/userdeck/:username", async (request, response) => {
  let connection = null;

  try {
      connection = await connectToDB();
      const { username } = request.params;

      if (!username) {
          return response.status(400).json({ error: 'Username is required' });
      }

      const [results, fields] = await connection.execute(`
      SELECT 
        dc.cardID, dc.card_times 
      FROM 
        deckCard dc 
        JOIN deck d ON dc.deckID = d.deckID 
        JOIN player p ON d.owner = p.username 
        WHERE p.username = ?`, [username]);

      response.status(200).json(results);
  } catch (error) {
      console.log(error);
      response.status(500).json(error);
  } finally {
      if (connection !== null) {
          await connection.end();
          console.log("Connection closed successfully");
      }
  }
});

app.post("/game/:username", async (request, response) => {
    let connection = null;
    try {
        connection = await connectToDB();
        const { username } = request.params;
        const { score, gameRound, kronos, deckCards } = request.body;

        const [results, fields] = await connection.query(
            "CALL PostGame(?, ?, ?, ?, ?)",
            [username, score, gameRound, kronos, deckCards]
        );

        console.log("Game posted successfully");
        response.status(200).send("Game posted successfully");
    } catch (error) {
        console.log(`Received body: ${request.body}`);
        console.log(error);
        response.status(500).json(error);
    } finally {
        if (connection !== null) {
            await connection.end();
            console.log("Connection closed succesfully");
        }
    }
});

app.get("/tophighscores", async (request, response) => {
  let connection = null;

  try {
      connection = await connectToDB();

      const [results, fields] = await connection.execute(`
      SELECT 
        username, score
      FROM game 
      ORDER BY score DESC LIMIT 10`);

      response.status(200).json(results);
  } catch (error) {
      console.log(error);
      response.status(500).json(error);
  } finally {
      if (connection !== null) {
          await connection.end();
          console.log("Connection closed successfully");
      }
  }
});

app.get("/enemy/wave/:waveID", async (request, response) => {
  let connection = null;

  try {
      connection = await connectToDB();
      const waveID = request.params.waveID;

      if (!waveID) {
          return response.status(400).json({ error: 'Wave ID is required' });
      }

      const [results, fields] = await connection.execute(`
      SELECT 
        cardID, card_times 
      FROM 
        enemyWave 
      WHERE roundID = ?`, [waveID]);

      response.status(200).json(results);
  } catch (error) {
      console.log(error);
      response.status(500).json(error);
  } finally {
      if (connection !== null) {
          await connection.end();
          console.log("Connection closed successfully");
      }
  }
});
