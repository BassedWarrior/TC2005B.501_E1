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


app.get('/EnemyWaves', (req, res) =>{

  let sql = 'SELECT * FROM EnemyWave';
  db.query(sql, (err, result) =>{
    if (err){
      res.status(500).send(err);return;}
    res.json(result);});});




app.listen(port, () => {
  console.log(`Server running on http://localhost:${port}/`);
});
