namespace NServiceBus.Unicast.Queuing.ActiveMQ.Config
{
    using Apache.NMS;
    using Apache.NMS.ActiveMQ;

    using NServiceBus.Unicast.Queuing.Installers;

    public static class ConfigureActiveMqMessageQueue
    {
        /// <summary>
        /// Use MSMQ for your queuing infrastructure.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Configure ActiveMqTransport(this Configure config, string consumerName, string connectionString)
        {
            config.Configurer.ConfigureComponent<ActiveMqMessageReceiver>(DependencyLifecycle.InstancePerCall)
                .ConfigureProperty(p => p.PurgeOnStartup, ConfigurePurging.PurgeRequested)
                .ConfigureProperty(p => p.ConsumerName, consumerName);

            config.Configurer.ConfigureComponent<ActiveMqMessageSender>(DependencyLifecycle.InstancePerCall);
            config.Configurer.ConfigureComponent<ActiveMqSubscriptionStorage>(DependencyLifecycle.InstancePerCall);
            
            config.Configurer.ConfigureComponent<SubscriptionManager>(DependencyLifecycle.SingleInstance);
            config.Configurer.ConfigureComponent<ActiveMqMessageMapper>(DependencyLifecycle.InstancePerCall);
            config.Configurer.ConfigureComponent<TopicEvaluator>(DependencyLifecycle.InstancePerCall);

            config.Configurer.ConfigureComponent<ActiveMqQueueCreator>(DependencyLifecycle.InstancePerCall);
            config.Configurer.ConfigureComponent<ActiveMqMessageDequeueStrategy>(DependencyLifecycle.InstancePerCall);
            config.Configurer.ConfigureComponent<NotifyMessageReceivedFactory>(DependencyLifecycle.InstancePerCall);

            var factory = new NetTxConnectionFactory(connectionString)
                {
                    AcknowledgementMode = AcknowledgementMode.Transactional,
                    PrefetchPolicy = { QueuePrefetch = 1 }
                };

            config.Configurer.ConfigureComponent<INetTxConnectionFactory>(() => factory, DependencyLifecycle.SingleInstance);
            config.Configurer.ConfigureComponent(() => (INetTxConnection)factory.CreateConnection(), DependencyLifecycle.SingleInstance);

            EndpointInputQueueCreator.Enabled = true;

            return config;
        }
    }
}
