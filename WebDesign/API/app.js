"use strict";

import express from "express";
import mysql from "mysql2/promise";
import dotenv from "dotenv/config"
import path from "path";

const app = express();
const port = process.env.PORT;  
app.use(express.json());

async function connectToDB() {
  return await mysql.createConnection({

    host: process.env.DB_HOST,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_DATABASE,
  });
}

app.get("/", async (req, res)=>{
  res.status(200).send("ADAD")
});


app.listen(port, () => {
  console.log(`Server running on http://localhost:${port}/`);
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
        const [results, fields] = await connection.execute("unitCards");

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

app.get("/enemy/wave/:waveID", async (request, response) => {
  let connection = null;
  const waveID = request.params.waveID;

  try {
    connection = await connectToDB();
    const [results, fields] = await connection.execute("SELECT * FROM EnemyWave WHERE roundID LIKE ?", [waveID]);
    console.log(`${results.length} rows returned`);
    const result = { cards: results };
    console.log(result);
    response.status(200).json(result);
  } catch (error) {
    console.log(error);
    response.status(500).json({ error: error.message });
  } finally {
    if (connection !== null) {
      await connection.end();
      console.log("Connection closed successfully");
    }
  }
});