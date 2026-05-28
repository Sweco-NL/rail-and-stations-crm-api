# Rail & Stations CRM Api

Dit project is een C# back-end voor de [RailAndStationsCrm](https://github.com/Sweco-NL/RailAndStationsCRM) repository, specifiek voor de nog niet gemergde branch [feature/example-asp-net-backend](https://github.com/Sweco-NL/RailAndStationsCrm/tree/feature/example-asp-net-backend).

Merk op: Dit project is een fork van [SupportTicket](https://github.com/MicrosoftDocs/aspire-docs-samples/fork). Dit is ook de reden dat de naam van een aantal van de projecten in deze solution met `SupportTicketApi` beginnen. Het hernoemen van de projecten vergt veel handwerk; wellicht kan de solution beter door een nieuwe vervangen worden, indien dit gewenst is.

<details>

<summary>Inhoudsopgave</summary>

- [Gebruikte Tooling](#gebruikte-tooling)
- [Opstarten](#opstarten)
  - [Ontwikkelomgeving](#ontwikkelomgeving)
    - [Back-end](#back-end)
    - [Database (Docker)](#database-docker)
  - [Productieomgeving](#productieomgeving)
  - [Back-end](#back-end-1)
  - [Database](#database)
- [Ontwikkelen](#ontwikkelen)
  - [Back-end](#back-end-2)
    - [API testen met Swagger](#api-testen-met-swagger)
    - [Aanpassen model (migraties)](#aanpassen-model-migraties)
  - [Database (Docker)](#database-docker-1)
    - [Starten](#starten)
    - [Stoppen](#stoppen)
    - [Inspectie van database in de terminal](#inspectie-van-database-in-de-terminal)

</details>

## Gebruikte Tooling

In dit hoofdstuk worden de gebruikte frameworks en libraries beschreven.

- [![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge)](https://dotnet.microsoft.com/en-us/)
- [![Entity Framework Core](https://img.shields.io/badge/EF%20Core-512BD4?logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAQAAAAEACAMAAABrrFhUAAABe2lDQ1BJQ0MgUHJvZmlsZQAAKM+Vkc8rRFEUxz9jRuRHQywsqEnDakZj1MTGYiaGwmKMMtjMvPml5sfrvZEmW2U7RYmNXwv+ArbKWikiJVvWxAY955mpkZqFe7vnfO733nO651yoC2eUrG7zQDZX0EJBv2M+suBoeMJGD+146Iwqujo9Ox6m5ni/xWL6a7eZi/+N5nhCV8DSKDyqqFpBeEJ4arWgmrwl3Kmko3HhE2GXJg8UvjH1WJmfTU6V+dNkLRwKSG1two7UL479YiWtZYWlcpzZzIpSeY9ZSUsiNzcrvldWNzohgvhxMMkYAXwMMiLWhxsvA7KjRrznJ36GvMQqYlWKaCyTIk0Bl6grkj0hPil6QmaGotn/v33Vk0PecvYWP9Q/GsZrHzRswlfJMD4ODOPrEKwPcJ6rxuf3YfhN9FJVc+6BfR1OL6pabBvONqDrXo1q0R/JKqsumYSXY2iNQMcVNC2We1Y55+gOwmvyVZewswv9ct++9A2aHmf9drlE3QAAAGNQTFRFUSvUdFXdfWDfXzzYWDPWdFbd3NX2////iG7ia0va3dX3y8DyZkXZfF/fYkDYxLfwakna7ur7eVveiW/i8/H88e77xbjxz8Xz6OP5uanuclTcoo7ok3vkgWXg0MbzXTnXVzLWpYtDKgAAAAlwSFlzAAAOwwAADsMBx2+oZAAAABh0RVh0U29mdHdhcmUAUGFpbnQuTkVUIDUuMS45bG7aPgAAAIxlWElmSUkqAAgAAAAFABoBBQABAAAASgAAABsBBQABAAAAUgAAACgBAwABAAAAAgAAADEBAgAQAAAAWgAAAGmHBAABAAAAagAAAAAAAABgAAAAAQAAAGAAAAABAAAAUGFpbnQuTkVUIDUuMS45AAIAAJAHAAQAAAAwMjMwAaADAAEAAAD//wAAAAAAAIGHzdx6YoqzAAACJklEQVR4Xu3duW5cMQyGUSexszn7vifv/5Rp/oKAVMwdQhMNck5LmZf4Shf2DQAAAAAAAAAAAAAAAAAAAADwf3nwsO1RVg1u86DhLqvWefyk7WlWDZ7lQcPzrFpHgHypQYCsGgiQBw0CZNU6AuRLDQJk1UCAPGgQIKvWESBfahAgqwYC5EGDAFm1ziTA/YtjbrNqMAvwMj90oldZtc4kwOuM2mYB3mS2DQFyWCFARm0CZGElQGbbECCHFQJk1CZAFlYCZLYNAXJYIUBGbQJkYSVAZtsQIIcVAmTUJkAWVgJktg0BclghQEZtAmRhJUBm2xAghxUCZNQmQBZWAmS2jUmAt/cHvMuemWsNcMj77JkRIG8qATLbhgA57FxHA3z4eLpP2bPUpQMc8Tl7lhIgHzuXANkzI0DenEuA7FlKgHzsXAJkz4wAeXOufxXgy9cDvmXPzLUGWPoboSMEyJ6lBMjHCgEyapsF+P7jdD+zZ6lLB7iG3wgJkFGbAFlYCZDZNgTIYYUAGbUJkIWVAJltQ4AcVgiQUZsAWVgJkNk2BMhhhQAZtQmQhZUAmW1DgBxWCJBRmwBZWAmQ2TYEyGGFABm1CZCFlQCZbUOAHFb8ujvmd1YNrjXAUYf+rK4AmW1DgBzWIEBWDQTIg0qAzLYhQA5rECCrBgLkQSVAZtsQIIc1CJBVAwHyoNouwKX/7e6fzAAAAAAAAAAAAAAAAAAAAAAARjc3fwG3VQVn/NvfXAAAAABJRU5ErkJggg==&logoColor=fff&style=for-the-badge)](https://learn.microsoft.com/en-us/ef/core/)
- [![Swagger](https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=173647&style=for-the-badge)](https://swagger.io/)
- [![Postgres](https://img.shields.io/badge/Postgres-%23316192.svg?logo=postgresql&logoColor=white&style=for-the-badge)](https://www.postgresql.org/)

## Opstarten

Om deze applicatie te draaien, moeten twee afzonderlijke onderdelen gestart worden: de back-end en de database. Dit hoofdstuk legt uit hoe deze gestart worden voor de ontwikkel- en de productieomgeving.

### Ontwikkelomgeving

#### Back-end

> Merk op, de projecten heten vanwege de oorsprong in de fork `SupportTicket`. Op termijn zal dit voor de duidelijk aangepast moeten worden naar iets relevanters.

1. Open het `CrmApi.slnx` project in een IDE als Visual Studio.
2. Pas de `ConnectionStrings` waarde in het `SupportTicketApi.AppHost/appsettings.json` bestand aan zodat deze overeenkomt met de database (zie hoofdstuk [Database](#database)).
3. Start het `SupportTicketApi.AppHost` project. Het volgende gebeurt:
   1. Een Aspire dashboard opent op `localhost:17152`. Hier is bij de service `api` onder `URLs` de URL van de API te vinden.
   2. De migraties voor de database worden automatisch uitgevoerd. Hierin wordt de database ook geseed (gevuld met standaard/dummy data).

#### Database (Docker)

> We gebruiken hier Docker om de database te starten vanwege het grote gemakt waarmee dit kan.

0. _(Alleen de eerste keer uitvoeren)_ Maak de database container genaamd `rail-and-stations-crm-db` met wachtwoord `mysecretpassword` en database `mydatabase` het volgende commando:

   ```bash
   docker run --name rail-and-stations-crm-db -e POSTGRES_PASSWORD=mysecretpassword -d -p 5432:5432 mydatabase
   ```

1. Start de container met commando:

   ```bash
   docker container start rail-and-stations-crm-db
   ```

2. De container kan gestopt worden met commando:

   ```bash
   docker container stop rail-and-stations-crm-db
   ```

### Productieomgeving

> [!WARNING]
> Dit hoofdstuk is niet afgerond.

### Back-end

...

### Database

...

## Ontwikkelen

Dit hoofdstuk bevat uitleg en geheugensteuntjes voor het ontwikkelen.

### Back-end

#### API testen met Swagger

De ontwikkelomgeving heeft een Swagger interface om met de API te communiceren. Dit kan bij het testen handig zijn. Merk op dat, indien beveiligde routes aangeroepen worden, de gebruiker "ingelogd" moet zijn. Bij Swagger doe je dit door een authorization token in te vulllen bij de "Authorize" knop. Deze token wordt gelogd in de console (waar de applicatie draait, niet in de browser) bij het inloggen op de front-end.

#### Aanpassen model (migraties)

Bij aanpassingen van het model van het project, kan het nodig zijn om een database migratie te maken. Deze mgiraties passen de database aan en maken het mogelijk bestaande databases in een correcte staat te houden zonder deze weg te hoeven gooien.

Een migratie wordt gemaakt met het volgende commando:

```
dotnet ef migrations add <NaamMigratie> --project ./SupportTicketApi.Data/SupportTicketApi.Data.csproj --startup-project ./SupportTicketApi.MigrationService/SupportTicketApi.MigrationService.csproj
```

Geef de migratie een passende naam. Als de database niet gevonden kan worden, bekijken dan over de connection string in `appsettings.json` bestand in `SupportTicketApi.Api` klopt.

### Database (Docker)

#### Starten

```bash
docker container start rail-and-stations-crm-db
```

#### Stoppen

```bash
docker container stop rail-and-stations-crm-db
```

#### Inspectie van database in de terminal

- Om een terminal _in_ de container te openen, voer het volgende commando uit:

  ```bash
  docker exec -it rail-and-stations-crm-db /bin/bash
  ```

- Gebruik nu het volgende commando om in de database te komen met de `psql` tool:

  ```bash
  psql -U postgres -d mydatabase
  ```

  Gebruik nu commando `\dt` om de tabellen van de database in te kunnen zien. Verder kunnen hier SQL commando's uitgevoerd worden.
