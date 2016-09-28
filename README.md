### Host your service
            // Initialize new instance of RemoteServiceContainer
            var container = new RemoteServiceContainer();

            // Register MyCustomService as IMyCustomService 
            container.RegisterService(typeof(IMyCustomService), new MyCustomService());

            // Open connection 
            container.Open(serverIp, port);

### Initialize service proxy in client side
            // Create instance of ServiceChannel
            var servicesChannel = new ServiceChannel(serverIp, port);

            // Generate remote service proxy
            var proxy = servicesChannel.GetRemoteService<IMyCustomService>();

### Use your service
            // Do some work in the server context
            proxy.DoSomething();
            
https://github.com/imatitya/netcorersi/wiki 
