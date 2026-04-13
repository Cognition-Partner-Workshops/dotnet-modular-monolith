package com.mycompany.mymeetings.buildingblocks.domain;

import java.time.LocalDateTime;
import java.util.UUID;

public abstract class AbstractDomainEvent implements DomainEvent {

    private final UUID id;
    private final LocalDateTime occurredOn;

    protected AbstractDomainEvent() {
        this.id = UUID.randomUUID();
        this.occurredOn = LocalDateTime.now();
    }

    @Override
    public UUID getId() {
        return id;
    }

    @Override
    public LocalDateTime getOccurredOn() {
        return occurredOn;
    }
}
