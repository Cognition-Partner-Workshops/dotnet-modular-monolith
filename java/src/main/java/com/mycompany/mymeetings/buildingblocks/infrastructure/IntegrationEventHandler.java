package com.mycompany.mymeetings.buildingblocks.infrastructure;

public interface IntegrationEventHandler<T extends IntegrationEvent> {

    void handle(T event);
}
