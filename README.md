# ByteLock

ByteLock is a secure, command-line file encryption and decryption tool written in C#. It allows users to protect sensitive files using AES encryption and verify file integrity with SHA256 checksums. Designed for simplicity, portability, and strong cryptographic security, ByteLock is ideal for personal data protection or inclusion in a cybersecurity toolkit.

## Features

- AES-256 file encryption and decryption
- SHA256 file checksums for integrity verification
- Supports encrypting/decrypting any file type
- Clean command-line interface (CLI)
- Output and vault directories are user-specific and excluded from version control

## Getting Started

### Prerequisites

- .NET SDK 6.0 or later (https://dotnet.microsoft.com/download)

### Clone the Repository

git clone https://github.com/ryanfmedeiros/ByteLock.git
cd ByteLock

### Build & Run

dotnet build
dotnet run

### Example Usage

dotnet run encrypt path/to/myfile.txt
dotnet run decrypt path/to/myfile.txt.enc
dotnet run hash path/to/myfile.txt

## Security Notes

Files are encrypted using AES with randomly generated 256-bit keys and IVs.
SHA256 hashes are generated per file for integrity checking.
Keys are currently not persisted; encryption is single-session based unless adapted.

## Future Plans

GUI version (WPF-based) for ease of use
Configurable encryption key storage
File overwrite protection and logging

## Author

Created by Ryan Medeiros (https://github.com/ryanfmedeiros)
