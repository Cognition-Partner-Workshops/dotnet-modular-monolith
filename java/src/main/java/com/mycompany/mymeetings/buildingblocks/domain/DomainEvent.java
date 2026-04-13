package com.mycompany.mymeetings.buildingblocks.domain;

import java.time.LocalDateTime;
import java.util.UUID;

public interface DomainEvent {

    UUID getId();

    LocalDateTime getOccurredOn();
}
