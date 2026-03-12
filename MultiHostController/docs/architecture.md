# System Architecture

## Overview

MultiHostController is a simplified distributed deployment system designed to demonstrate how a central controller can manage and execute tasks across multiple hosts.

The system consists of two main components:

* **MasterController** – central orchestration server
* **ClientAgent** – lightweight agent running on managed hosts

The architecture separates the **control plane** from the **execution plane**, similar to real-world orchestration systems such as Ansible, SaltStack, or Kubernetes agents.

------------------------------------------------------------------------------------------

# High-Level Architecture

+-------------------+
|   MasterController|
|  ASP.NET Core API |
+---------+---------+
          |
          | REST API
          |
+---------v---------+
|    ClientAgent    |
| .NET Worker Agent |
+---------+---------+
          |
          | Executes
          |
+---------v---------+
| Deployment Script |
|  install-minio.ps1|
+-------------------+

------------------------------------------------------------------------------------------

# Component Responsibilities

## MasterController

The MasterController is responsible for orchestration and system coordination.

Responsibilities:

* Host registration
* Heartbeat monitoring
* Task scheduling
* Task result collection
* Persistent storage of system state

Technology stack:

* ASP.NET Core Web API
* Entity Framework Core
* SQLite database
* Swagger API documentation

------------------------------------------------------------------------------------------

## ClientAgent

The ClientAgent runs on each managed host and communicates with the MasterController.

Responsibilities:

* Register host with the controller
* Send periodic heartbeat signals
* Poll the controller for assigned tasks
* Execute deployment scripts
* Report task results

Technology stack:

* .NET Worker Service
* HttpClient
* PowerShell process execution

------------------------------------------------------------------------------------------

# Communication Model

Communication between the agent and controller uses REST APIs.

### Host Registration

    POST /api/hosts/register

Registers a new host with the MasterController.

------------------------------------------------------------------------------------------

### Heartbeat

    POST /api/hosts/heartbeat/{id}

Used by the ClientAgent to signal that the host is still active.

------------------------------------------------------------------------------------------

### Task Assignment

    POST /api/tasks

Creates a new deployment task for a specific host.

------------------------------------------------------------------------------------------

### Task Polling

    GET /api/tasks/{hostId}

ClientAgent periodically polls the controller to retrieve pending tasks.

------------------------------------------------------------------------------------------

### Task Completion

    POST /api/tasks/result

Agent reports execution results back to the controller.

------------------------------------------------------------------------------------------

# Database Design

SQLite is used to store system state.

## Hosts Table

| Column        | Description                 |
| ------------- | --------------------------- |
| Id            | Host identifier             |
| Hostname      | Machine name                |
| IpAddress     | Host IP                     |
| OS            | Operating system            |
| LastHeartbeat | Timestamp of last heartbeat |

------------------------------------------------------------------------------------------

## Tasks Table

| Column    | Description        |
| --------- | ------------------ |
| Id        | Task identifier    |
| HostId    | Target host        |
| Command   | Deployment command |
| Status    | Task status        |
| CreatedAt | Creation timestamp |

------------------------------------------------------------------------------------------

# Deployment Workflow

1. ClientAgent starts on a host
2. Agent registers with MasterController
3. Agent sends periodic heartbeats
4. Controller assigns deployment tasks
5. Agent polls for tasks
6. Agent executes deployment script
7. Agent reports execution result
8. Controller stores task result in database

------------------------------------------------------------------------------------------

# Design Considerations

### Lightweight Agent

The agent is designed to be simple and lightweight, allowing it to run on multiple machines without heavy dependencies.

### Polling-Based Task Retrieval

Agents poll the controller periodically to retrieve tasks. This approach simplifies networking and avoids the need for persistent connections.

### Script-Based Deployment

Deployment commands are executed using scripts, allowing flexible automation without requiring changes to the agent code.

------------------------------------------------------------------------------------------

# Future Improvements

Potential enhancements for a production system include:

* Authentication between agent and controller
* Secure communication (TLS + API keys)
* Task retry mechanisms
* Centralized deployment logs
* Web dashboard for monitoring hosts and tasks
* Container-based deployments
