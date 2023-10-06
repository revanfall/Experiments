
# Introduction
This project implements a simple REST API consisting of two endpoints to facilitate the experiments. The API allows clients to request experiments and gather data based on device tokens.

# API Description
## Get Experiment
Type: GET
Parameters: device-token (string)
Return: JSON response {key: "X-name", value: "string"}

# Experiment Details
## Button Color Experiment
### Key: button_color
Options:
#FF0000,
#00FF00,
#0000FF
## Price Experiment
### Key: price
Options:
10,
20,
50,
5

# Requirements and Limitations
1. Once a device receives a value, it always gets the same value.
2. Experiments are conducted only for new devices.

# Database Schema
The project uses an MS SQL database to store experiment information and results. Below is the schema:
![Db_diagram](https://github.com/revanfall/Experiments/assets/45764065/8023d362-eda4-4249-b500-3c387c56d7ad)


# Technologies Used
- .NET Framework
- MS SQL Database
- Swagger for API Documentation
- Serilog

