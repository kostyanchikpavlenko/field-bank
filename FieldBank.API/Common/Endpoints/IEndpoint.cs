﻿namespace FieldBank.API.Common.Endpoints
{
    public interface IEndpoint
    { 
        void MapEndpoint(IEndpointRouteBuilder app); 
    }
}