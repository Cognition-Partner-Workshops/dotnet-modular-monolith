package com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals.acceptmeetinggroupproposal;

import com.mycompany.mymeetings.buildingblocks.infrastructure.EventsBus;
import com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals.events.MeetingGroupProposalAcceptedDomainEvent;
import com.mycompany.mymeetings.modules.administration.integrationevents.MeetingGroupProposalAcceptedIntegrationEvent;
import org.springframework.context.event.EventListener;
import org.springframework.stereotype.Component;

import java.util.UUID;

@Component
public class MeetingGroupProposalAcceptedNotificationHandler {

    private final EventsBus eventsBus;

    public MeetingGroupProposalAcceptedNotificationHandler(EventsBus eventsBus) {
        this.eventsBus = eventsBus;
    }

    @EventListener
    public void handle(MeetingGroupProposalAcceptedDomainEvent domainEvent) {
        eventsBus.publish(new MeetingGroupProposalAcceptedIntegrationEvent(
                UUID.randomUUID(),
                domainEvent.getOccurredOn(),
                domainEvent.getMeetingGroupProposalId().getValue()));
    }
}
