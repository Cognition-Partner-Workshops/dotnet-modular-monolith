package com.mycompany.mymeetings.modules.administration.application.members;

import com.mycompany.mymeetings.buildingblocks.infrastructure.IntegrationEvent;

import java.time.LocalDateTime;
import java.util.UUID;

public class NewUserRegisteredIntegrationEvent extends IntegrationEvent {

    private final UUID userId;
    private final String login;
    private final String email;
    private final String firstName;
    private final String lastName;
    private final String name;

    public NewUserRegisteredIntegrationEvent(
            UUID id,
            LocalDateTime occurredOn,
            UUID userId,
            String login,
            String email,
            String firstName,
            String lastName,
            String name) {
        super(id, occurredOn);
        this.userId = userId;
        this.login = login;
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
        this.name = name;
    }

    public UUID getUserId() { return userId; }
    public String getLogin() { return login; }
    public String getEmail() { return email; }
    public String getFirstName() { return firstName; }
    public String getLastName() { return lastName; }
    public String getName() { return name; }
}
