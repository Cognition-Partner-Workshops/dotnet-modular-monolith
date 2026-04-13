package com.mycompany.mymeetings.modules.administration.application.members;

import com.mycompany.mymeetings.modules.administration.application.contracts.CommandsScheduler;
import com.mycompany.mymeetings.modules.administration.application.members.createmember.CreateMemberCommand;
import org.springframework.context.event.EventListener;
import org.springframework.stereotype.Component;

import java.util.UUID;

@Component
public class NewUserRegisteredIntegrationEventHandler {

    private final CommandsScheduler commandsScheduler;

    public NewUserRegisteredIntegrationEventHandler(CommandsScheduler commandsScheduler) {
        this.commandsScheduler = commandsScheduler;
    }

    @EventListener
    public void handle(NewUserRegisteredIntegrationEvent event) {
        commandsScheduler.enqueue(
                new CreateMemberCommand(
                        UUID.randomUUID(),
                        event.getUserId(),
                        event.getLogin(),
                        event.getEmail(),
                        event.getFirstName(),
                        event.getLastName(),
                        event.getName()));
    }
}
