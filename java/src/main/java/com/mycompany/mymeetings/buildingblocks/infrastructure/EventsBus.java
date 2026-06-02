package com.mycompany.mymeetings.buildingblocks.infrastructure;

public interface EventsBus {

    void publish(IntegrationEvent event);

    <T extends IntegrationEvent> void subscribe(Class<T> eventType, IntegrationEventHandler<T> handler);
}
