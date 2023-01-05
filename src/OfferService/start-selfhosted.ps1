dapr run `
    --app-id offerservice `
    --app-port 5115 `
    --dapr-http-port 3600 `
    --dapr-grpc-port 60500 `
    --config ../dapr/components/config.yaml `
    --components-path ../dapr/components `
    dotnet run