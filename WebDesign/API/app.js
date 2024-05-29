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

app.get("/", async (req, res)=>{
  res.status(200).send("ADAD")
});
  
app.get("/cards", async (request, response) => {
  let connection = null;

  try {
    connection = await connectToDB();
    const [results, fields] = await connection.execute("select * from card");

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

app.listen(port, () => {
  console.log(`Server running on http://localhost:${port}/`);
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

      // Llamar al procedimiento almacenado de Sign In
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