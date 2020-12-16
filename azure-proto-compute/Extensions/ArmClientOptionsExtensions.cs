﻿using Azure.ResourceManager.Core;

namespace azure_proto_compute.Extensions
{
    public static class ArmClientOptionsExtensions
    {
        public static ComputeRestApiVersions ComputeRestVersions(this ArmClientOptions armClientOptions)
        {
            return armClientOptions.GetOverrideObject<ComputeRestApiVersions>(() => new ComputeRestApiVersions()) as ComputeRestApiVersions;
        }
    }
}
