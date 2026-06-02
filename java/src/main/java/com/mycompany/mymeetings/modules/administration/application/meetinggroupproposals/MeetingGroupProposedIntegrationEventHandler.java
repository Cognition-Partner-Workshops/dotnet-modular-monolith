package com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals;

import com.mycompany.mymeetings.modules.administration.application.contracts.CommandsScheduler;
import com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals.requestmeetinggroupproposalverification.RequestMeetingGroupProposalVerificationCommand;
import org.springframework.context.event.EventListener;
import org.springframework.stereotype.Component;

import java.util.UUID;

@Component
public class MeetingGroupProposedIntegrationEventHandler {

    private final CommandsScheduler commandsScheduler;

    public MeetingGroupProposedIntegrationEventHandler(CommandsScheduler commandsScheduler) {
        this.commandsScheduler = commandsScheduler;
    }

    @EventListener
    public void handle(MeetingGroupProposedIntegrationEvent event) {
        commandsScheduler.enqueue(
                new RequestMeetingGroupProposalVerificationCommand(
                        UUID.randomUUID(),
                        event.getMeetingGroupProposalId(),
                        event.getName(),
                        event.getDescription(),
                        event.getLocationCity(),
                        event.getLocationCountryCode(),
                        event.getProposalUserId(),
                        event.getProposalDate()));
    }
}
