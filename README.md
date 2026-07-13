# CodeWithMixx

CodeWithMixx is a Razor Pages web application built in C# and .NET 10 for promoting tutoring and helping students who are struggling with IT related subjects.

The idea behind the project is simple. Learning IT can feel overwhelming when you are stuck on exam prep, homework, or project deadlines, so this site presents a more personal and practical way to get support. It is written from the perspective of someone who has been through the same path and wants to make the process easier for others.

## What this project is about

CodeWithMixx is a landing style tutoring platform focused on individual lessons and exam preparation in IT topics. It presents tutoring in a straightforward way, with sections for the story behind the project, the services offered, the workflow, and pricing.

The main goal is to help students with topics such as:

- Programming fundamentals
- Object oriented programming
- Backend development
- Frontend development
- Databases
- Exam preparation and practical problem solving

The site is designed to feel direct and personal, with an emphasis on real help instead of heavy theory.

## Tech stack

- C#
- ASP.NET Razor Pages
- .NET 10
- HTMX
- JavaScript
- Tailwind CSS
- PostgreSQL

## Project structure

The app is organized around Razor Pages and shared partials.

A few important parts of the structure are:

- `Pages/`  
  Main Razor Pages for the application

- `Pages/Shared/`  
  Shared layout and reusable components

- `Pages/Shared/_Partials/`  
  Common page sections like the header and footer

- `Pages/Shared/_Partials/Landing/`  
  Landing page sections such as:
  - Hero
  - About
  - Services
  - How it works
  - Pricing

- `wwwroot/`  
  Static assets such as styles, scripts, and images

The landing page is split into partials so the content is easier to maintain and update.

## Content overview

The landing page currently includes:

- A hero section with a clear call to action
- A personal story section
- A services section that explains what students can learn
- A process section that shows how tutoring works
- A pricing section with package options

## Upcoming features
- A dashboard for student's where every individual student will be able to track his own classes
- Retrieve and submit homework
- View additional metrics
