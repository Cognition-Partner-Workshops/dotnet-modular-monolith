using Autofac;
using CompanyName.MyMeetings.Modules.Orders.Application.Contracts;
using CompanyName.MyMeetings.Modules.Orders.Infrastructure.Persistence;
using CompanyName.MyMeetings.Modules.Orders.Infrastructure.Services;

namespace CompanyName.MyMeetings.API.Modules.Orders;

public class OrdersAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<JsonOrderRepository>()
            .As<IOrderRepository>()
            .SingleInstance();

        builder.RegisterType<OrderService>()
            .As<IOrderService>()
            .InstancePerLifetimeScope();
    }
}
