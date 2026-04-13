package com.mycompany.mymeetings.buildingblocks.infrastructure;

import java.time.LocalDateTime;
import java.util.UUID;

public abstract class IntegrationEvent {

    private final UUID id;
    private final LocalDateTime occurredOn;

    protected IntegrationEvent(UUID id, LocalDateTime occurredOn) {
        this.id = id;
        this.occurredOn = occurredOn;
    }

    public UUID getId() {
        return id;
    }

    public LocalDateTime getOccurredOn() {
        return occurredOn;
    }
}
