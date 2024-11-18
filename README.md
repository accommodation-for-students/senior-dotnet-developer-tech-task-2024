# Senior .NET Developer Tech Task 2024

## Context

Imagine you are working in a mature codebase for a student property marketplace and are following best practice. You have been tasked with creating the application logic for an automated job that will ingest a feed of properties and persist them to the database. Data validity is important and the rules in the `rules` section should be implemented, the system should not be able to get into an invalid state.

You are free to model this however you wish, however approach it with the same rigour as you would with a production feature. You don't need to worry about how the job will be ran and only need to focus on the application logic.

## Todo

- Call an API, `https://www.studentproperties.com/api/properties`, this returns responses like the one in `./properties.json`
- Model the domain and map this response onto your model
- The rules in the `rules` section should be implemented
- It should be possible to update property photos, i.e. adding and removing them
- Once the properties have been mapped to the model, persist them to the database

## Constraints

- Written in C#
- You are free to model this however you like
- You can use any database technology, i.e. SQLite, SQL Server, in-memory

## Rules

- Studios can only have one bedroom
- Flats and houses must have at least 1 bedroom
- Flats and houses can have a maximum of 14 bedrooms
- Valid values for bed size are small, king size and double
- Valid values for room size are small, medium and large
- UK properties have their prices in GBP and Republic of Ireland properties have their prices in EUR
- Properties have a limit of 14 photos
- Our property pages show the "price from" price for each property, which is the price of the cheapest available bedroom, so it would be good if a property can calculate this somehow
- It should be possible to update the photos of a property, to simplify things these can be replaced all in one go at the same time