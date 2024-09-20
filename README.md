# CandidateHUB


## Overview

CandidateHubApi is a RESTful API developed using .NET Core for managing job candidate information. It allows adding and updating candidate profiles based on their email addresses. The application is designed with scalability and maintainability in mind, facilitating future migrations to different databases.

## Features

- **Add or Update Candidate:** Single endpoint to create or update candidate information using email as a unique identifier.
- **Caching:** Implements in-memory caching for efficient data retrieval.
- **Unit Tested:** Comprehensive unit tests for service and controller layers.
- **Swagger Integration:** Interactive API documentation and testing.

## Tech Stack

- **Framework:** .NET Core 8.0
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Testing:** xUnit, Moq
- **Caching:** In-Memory Cache
- **Version Control:** Git, GitHub

## Setup Instructions

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/MohammadAbuDaleh/CandidateHUB.git