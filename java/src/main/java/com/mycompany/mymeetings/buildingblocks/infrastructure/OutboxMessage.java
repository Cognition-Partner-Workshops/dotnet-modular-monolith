package com.mycompany.mymeetings.buildingblocks.infrastructure;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@Table(name = "OutboxMessages", schema = "administration")
public class OutboxMessage {

    @Id
    private UUID id;

    @Column(nullable = false)
    private LocalDateTime occurredOn;

    @Column(nullable = false)
    private String type;

    @Column(nullable = false, columnDefinition = "TEXT")
    private String data;

    @Column
    private LocalDateTime processedDate;

    protected OutboxMessage() {
    }

    public OutboxMessage(LocalDateTime occurredOn, String type, String data) {
        this.id = UUID.randomUUID();
        this.occurredOn = occurredOn;
        this.type = type;
        this.data = data;
    }

    public UUID getId() {
        return id;
    }

    public LocalDateTime getOccurredOn() {
        return occurredOn;
    }

    public String getType() {
        return type;
    }

    public String getData() {
        return data;
    }

    public LocalDateTime getProcessedDate() {
        return processedDate;
    }

    public void setProcessedDate(LocalDateTime processedDate) {
        this.processedDate = processedDate;
    }
}
