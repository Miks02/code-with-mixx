FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build --chown=app:app /app/publish ./

USER app

ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 10000

ENV PORT=10000
CMD