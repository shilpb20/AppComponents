# Changelog

All notable changes to this project will be documented in this file.  
This project follows [Semantic Versioning](https://semver.org/).

## [Unreleased]
### Added
- Placeholder for upcoming features

## [1.0.0] - 2025-02-24
### Added
- Initial release with Repository Pattern implementation
- Support for EF Core for database operations
- Support for CRUD operations including sorting, filtering, and pagination.
- Asynchronous methods for improved performance

## [1.1.0] - 2025-02-24
### Added
- Time-stamped repository to automatically track entity creation and modification times.

## [1.2.0] - 2025-03-15
### Added
- Transaction management interface to handle database transactions.
- Option to skip transaction behavior during tests using the 'UseInMemoryDatabase' configuration setting.
## [1.2.1] - 2025-03-15
### Added
- Transaction settings file added to configure transaction behavior (e.g., skip transactions in tests).

## [1.3.0] - 2025-03-21
### Added
- Documentation changes (clarified repository usage(in progress), added setup instructions, new Github path)
---

## 🔹 How to Use This Changelog

- **Major** (`X.0.0`): Breaking changes that require modifications in user code.  
- **Minor** (`0.X.0`): New features added in a backward-compatible manner.  
- **Patch** (`0.0.X`): Bug fixes and optimizations that do not change functionality.  

