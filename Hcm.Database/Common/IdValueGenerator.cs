using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Hcm.Database.Common
{
    public class IdValueGenerator : ValueGenerator
    {
        public override bool GeneratesTemporaryValues => false;

        protected override object NextValue([NotNull] EntityEntry entry)
        {
            return Guid.NewGuid()
                .ToString()
                .Replace("-", "");
        }
    }
}
