# Web Development Master Class

This repo contains the code used in my Web Development Master Class. 

## Getting started

To get started, you will need an IDE, as well as Docker Desktop installed, as it uses containers for a couple of the projects.

Before you can run the project, you need to configure an SSL certificate for the Identity Server project. To do this, you need a certificate that you trust. The easiest is to simply use the one that ASP.NET Core sets up for you.

To get hold of the ASP.NET Core certificate you need to do run the following command:

```bash
dotnet dev-certs https --export-path ./ssl-cert.pfx --password P@ssw0rd123!
```

By doing this, the certificate will automatically be added to the Identity Server container and used by Kestrel.

__Note:__ The code for this is in [the AppHost's Program.cs file](./src/WebDevMasterClass.AppHost/AppHost.cs)

Once these steps have been completed, you should be able to simply press F5 to run the solution. As long as the __WebDevMasterClass.AppHost__ project is set as start-up project.

## Questions

If you have any questions, feel free to reach out to me. I'm available on Twitter...sorry...X...at [@zerokoll](https://twitter.com/zerokoll) and on LinkedIn at [https://www.linkedin.com/in/zerokoll/](https://www.linkedin.com/in/zerokoll/).

## Interested in the Workshop?

If you are interested in the workshop, please reach out to me on Twitter or LinkedIn. I would be more than happy to come and run the workshop for you and your team.