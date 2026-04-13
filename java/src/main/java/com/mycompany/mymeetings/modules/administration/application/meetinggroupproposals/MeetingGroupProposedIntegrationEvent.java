package com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals;

import com.mycompany.mymeetings.buildingblocks.infrastructure.IntegrationEvent;

import java.time.LocalDateTime;
import java.util.UUID;

public class MeetingGroupProposedIntegrationEvent extends IntegrationEvent {

    private final UUID meetingGroupProposalId;
    private final String name;
    private final String description;
    private final String locationCity;
    private final String locationCountryCode;
    private final UUID proposalUserId;
    private final LocalDateTime proposalDate;

    public MeetingGroupProposedIntegrationEvent(
            UUID id,
            LocalDateTime occurredOn,
            UUID meetingGroupProposalId,
            String name,
            String description,
            String locationCity,
            String locationCountryCode,
            UUID proposalUserId,
            LocalDateTime proposalDate) {
        super(id, occurredOn);
        this.meetingGroupProposalId = meetingGroupProposalId;
        this.name = name;
        this.description = description;
        this.locationCity = locationCity;
        this.locationCountryCode = locationCountryCode;
        this.proposalUserId = proposalUserId;
        this.proposalDate = proposalDate;
    }

    public UUID getMeetingGroupProposalId() { return meetingGroupProposalId; }
    public String getName() { return name; }
    public String getDescription() { return description; }
    public String getLocationCity() { return locationCity; }
    public String getLocationCountryCode() { return locationCountryCode; }
    public UUID getProposalUserId() { return proposalUserId; }
    public LocalDateTime getProposalDate() { return proposalDate; }
}
