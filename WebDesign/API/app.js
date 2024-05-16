"use strict";

import express from "express";
import mysql from "mysql2/promise";

const app = express();
const port = 4321;

app.use(express.json());

async function connectToDB() {
  return await mysql.createConnection({
    host: "localhost",
    user: "porto1090",
    password: "asdf1234",
    database: "chronical-doom",
  });
}

app.get("/", async (req, res)=>{
  res.status(200).send("ADAD")
});
  
app.get("/api/cards", async (request, response) => {
  let connection = null;

  try {
    connection = await connectToDB();
    const [results, fields] = await connection.execute("select * from card");

    console.log(`${results.length} rows returned`);
    console.log(results);
    response.status(200).json(results);
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