﻿namespace NServiceBus.Features
{
    using MessageInterfaces.MessageMapper.Reflection;
    using Serializers.Json;

    /// <summary>
    /// Used to configure json as a message serializer
    /// </summary>
    public class JsonSerialization : Feature
    {
        
        internal JsonSerialization()
        {
        }

        /// <summary>
        /// See <see cref="Feature.Setup"/>
        /// </summary>
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Container.ConfigureComponent<MessageMapper>(DependencyLifecycle.SingleInstance);
            context.Container.ConfigureComponent<JsonMessageSerializer>(DependencyLifecycle.SingleInstance)
                 .ConfigureProperty(s => s.SkipArrayWrappingForSingleMessages, !context.Settings.GetOrDefault<bool>("SerializationSettings.WrapSingleMessages"));
        }
    }
}