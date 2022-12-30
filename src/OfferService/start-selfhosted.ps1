dapr run `
    --app-id offerservice `
    --app-port 5115 `
    --dapr-http-port 3600 `
    --dapr-grpc-port 60000 `
    --config ../dapr/config/config.yaml `
    --components-path ../dapr/components `
    dotnet run