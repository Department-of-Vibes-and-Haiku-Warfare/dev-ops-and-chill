# Developer Setup Guide

Welcome to the project! This document will guide you through the process of setting up your local development environment.

## Prerequisites

Before you start, ensure you have the following installed:

1. **VS Code**  
   We recommend using **Visual Studio Code** (VS Code) for development, as it provides excellent support for Docker, C#, and remote environments. Download VS Code from [here](https://code.visualstudio.com/Download).

2. **VS Code Remote-Containers Extension**  
   Install the **Remote-Containers** extension in VS Code to develop directly inside the containerized environment. You can find it in the [VS Code marketplace](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers).

3. **Docker**  
   Docker is required for running the remote development environment. Install Docker from [here](https://docs.docker.com/get-docker/).

## Setting Up the Development Environment

1. **Clone the Repository**  
   First, clone the project repository to your local machine:

   ```bash
   git clone https://github.com/your-org/your-repository.git
   ```

2. **Open the Project in VS Code**  
   Open the project folder in **VS Code**. If you have the **Remote-Containers** extension installed, you will be prompted to open the project inside the container. Select "Reopen in Container" to work directly inside the containerized environment.

   [VS Code Remote-Containers Documentation](https://code.visualstudio.com/docs/remote/containers)

3. **Running the Application**  
   Once inside the remote container, open the Run and Debug tab:

   Select "Haiku Launch (console)" and start debugging

4. **Debugging**  
   VS Code provides excellent debugging support for C#. Set breakpoints and use the VS Code debug panel to inspect and run the application directly from the editor.

   [VS Code Debugging C#](https://code.visualstudio.com/docs/languages/csharp)

## Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [VS Code Documentation](https://code.visualstudio.com/docs)
- [C# Development in VS Code](https://code.visualstudio.com/docs/languages/csharp)
- [Docker for VS Code](https://code.visualstudio.com/docs/containers/overview)
