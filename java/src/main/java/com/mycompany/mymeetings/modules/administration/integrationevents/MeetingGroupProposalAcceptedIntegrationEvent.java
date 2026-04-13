package com.mycompany.mymeetings.modules.administration.integrationevents;

import com.mycompany.mymeetings.buildingblocks.infrastructure.IntegrationEvent;

import java.time.LocalDateTime;
import java.util.UUID;

public class MeetingGroupProposalAcceptedIntegrationEvent extends IntegrationEvent {

    private final UUID meetingGroupProposalId;

    public MeetingGroupProposalAcceptedIntegrationEvent(
            UUID id,
            LocalDateTime occurredOn,
            UUID meetingGroupProposalId) {
        super(id, occurredOn);
        this.meetingGroupProposalId = meetingGroupProposalId;
    }

    public UUID getMeetingGroupProposalId() {
        return meetingGroupProposalId;
    }
}
