// <copyright file="TracerBuilderExtensions.cs" company="OpenTelemetry Authors">
// Copyright 2018, OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using Datadog.Trace.OpenTelemetry;
using OpenTelemetry.Trace.Configuration;
using OpenTelemetry.Trace.Export;

namespace OpenTelemetry.Exporter.Datadog
{
    /// <summary>
    /// Extension methods to configure the <see cref="DatadogExporter"/>.
    /// </summary>
    public static class TracerBuilderExtensions
    {
        /// <summary>
        /// Registers a Datadog exporter.
        /// </summary>
        /// <param name="builder">Trace builder to use.</param>
        /// <param name="configure">Exporter configuration options.</param>
        /// <returns>The instance of <see cref="TracerBuilder"/> to chain the calls.</returns>
        public static TracerBuilder UseDatadog(this TracerBuilder builder, Action<DatadogExporterOptions> configure)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var options = new DatadogExporterOptions();
            configure(options);
            return builder.AddProcessorPipeline(b => b
                .SetExporter(new DatadogExporter(options))
                .SetExportingProcessor(e => new SimpleSpanProcessor(e)));
        }

        /// <summary>
        /// Registers Datadog exporter.
        /// </summary>
        /// <param name="builder">Trace builder to use.</param>
        /// <param name="configure">Exporter configuration options.</param>
        /// <param name="processorConfigure">Span processor configuration.</param>
        /// <returns>The instance of <see cref="TracerBuilder"/> to chain the calls.</returns>
        public static TracerBuilder UseDatadog(this TracerBuilder builder, Action<DatadogExporterOptions> configure, Action<SpanProcessorPipelineBuilder> processorConfigure)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            if (processorConfigure == null)
            {
                throw new ArgumentNullException(nameof(processorConfigure));
            }

            var options = new DatadogExporterOptions();
            configure(options);
            return builder.AddProcessorPipeline(b =>
            {
                b.SetExporter(new DatadogExporter(options));
                processorConfigure.Invoke(b);
            });
        }
    }
}
