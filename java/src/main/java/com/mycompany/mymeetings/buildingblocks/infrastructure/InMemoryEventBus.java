package com.mycompany.mymeetings.buildingblocks.infrastructure;

import org.springframework.context.ApplicationEventPublisher;
import org.springframework.stereotype.Component;

@Component
public class InMemoryEventBus implements EventsBus {

    private final ApplicationEventPublisher applicationEventPublisher;

    public InMemoryEventBus(ApplicationEventPublisher applicationEventPublisher) {
        this.applicationEventPublisher = applicationEventPublisher;
    }

    @Override
    public void publish(IntegrationEvent event) {
        applicationEventPublisher.publishEvent(event);
    }

    @Override
    public <T extends IntegrationEvent> void subscribe(Class<T> eventType, IntegrationEventHandler<T> handler) {
        // Spring's @EventListener handles subscriptions declaratively
    }
}
