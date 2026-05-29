# MultiHostController

## Overview

MultiHostController is a lightweight orchestration system designed to manage and execute deployment tasks across multiple machines.

The system consists of two components:

* **MasterController** – central API server that manages hosts and deployment tasks
* **ClientAgent** – lightweight agent installed on target machines that executes tasks assigned by the MasterController

The system demonstrates a simplified distributed deployment architecture.

------------------------------------------------------------------------------------------

Core Architecture of This System:

1️.	Master Controller (API server)
* Host registration
* Heartbeat monitoring
* Task scheduling
* Task completion reporting
* Database persistence (SQLite)

2.	Client Agent
* Registers itself with the server
* Sends heartbeat
* Polls for tasks
* Executes deployment script
* Reports results

3.	Deployment Execution
* Runs PowerShell script (install-minio.ps1)
* Handles command execution

4.	Persistence Layer
* SQLite database
* Entity Framework Core
* Hosts + Tasks tables

5.	API Interface
* Swagger UI
* REST endpoints for orchestration

------------------------------------------------------------------------------------------

# Architecture

ClientAgent
    ↓ register
MasterController API
    ↓
Task Scheduler
    ↓
Agent Polling
    ↓
Execute Script
    ↓
Return Result
    ↓
SQLite Database

------------------------------------------------------------------------------------------

# Components

## MasterController

ASP.NET Core Web API responsible for:

* Host registration
* Heartbeat monitoring
* Task scheduling
* Task result processing
* Database persistence

Key Technologies:

* ASP.NET Core
* Entity Framework Core
* SQLite
* Swagger API documentation

------------------------------------------------------------------------------------------

## ClientAgent

Background worker service responsible for:

* Registering the host
* Sending heartbeat signals
* Polling tasks from the controller
* Executing deployment scripts
* Reporting task results

Key Technologies:

* .NET Worker Service
* HttpClient
* PowerShell execution

------------------------------------------------------------------------------------------

# Database

SQLite database used for persistence.

Tables:

## Hosts

| Field         | Description              |
| ------------- | ------------------------ |
| Id            | Host identifier          |
| Hostname      | Machine name             |
| IpAddress     | Host IP                  |
| OS            | Operating system         |
| LastHeartbeat | Last heartbeat timestamp |

## Tasks

| Field     | Description             |
| --------- | ----------------------- |
| Id        | Task identifier         |
| HostId    | Target host             |
| Command   | Command to execute      |
| Status    | Task status             |
| CreatedAt | Task creation timestamp |

------------------------------------------------------------------------------------------

# Running the System

## 1 Start MasterController

cd src/MasterController
dotnet run

Swagger UI:

http://localhost:5200/swagger

------------------------------------------------------------------------------------------

## 2 Start ClientAgent

	cd src/ClientAgent
	dotnet run

The agent will automatically:

* Register host
* Send heartbeat
* Poll tasks

------------------------------------------------------------------------------------------

# Create a Deployment Task

Using Swagger:

	POST /api/tasks

Example request (JSON):

	{
	  "hostId": 1,
	  "command": "install-minio"
	}

------------------------------------------------------------------------------------------

# Deployment Execution

The ClientAgent receives the task and runs:

	scripts/install-minio.ps1

After execution, the agent reports the result back to the MasterController.

------------------------------------------------------------------------------------------

# Design Decisions

SQLite was selected as the persistence layer to keep the system self-contained and easy to run without external dependencies.

The architecture separates the control plane (MasterController) and execution plane (ClientAgent) to simulate a real-world distributed deployment system.

------------------------------------------------------------------------------------------

# Future Improvements

Possible enhancements include:

* Authentication between agent and controller
* Task retry mechanism
* Deployment logs
* Web dashboard
* Support for multiple deployment commands
* Container deployment support

------------------------------------------------------------------------------------------
