package com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals.requestmeetinggroupproposalverification;

import com.mycompany.mymeetings.modules.administration.application.contracts.CommandBaseWithResult;

import java.time.LocalDateTime;
import java.util.UUID;

public class RequestMeetingGroupProposalVerificationCommand extends CommandBaseWithResult<UUID> {

    private final UUID meetingGroupProposalId;
    private final String name;
    private final String description;
    private final String locationCity;
    private final String locationCountryCode;
    private final UUID proposalUserId;
    private final LocalDateTime proposalDate;

    public RequestMeetingGroupProposalVerificationCommand(
            UUID id,
            UUID meetingGroupProposalId,
            String name,
            String description,
            String locationCity,
            String locationCountryCode,
            UUID proposalUserId,
            LocalDateTime proposalDate) {
        super(id);
        this.meetingGroupProposalId = meetingGroupProposalId;
        this.name = name;
        this.description = description;
        this.locationCity = locationCity;
        this.locationCountryCode = locationCountryCode;
        this.proposalUserId = proposalUserId;
        this.proposalDate = proposalDate;
    }

    public UUID getMeetingGroupProposalId() {
        return meetingGroupProposalId;
    }

    public String getName() {
        return name;
    }

    public String getDescription() {
        return description;
    }

    public String getLocationCity() {
        return locationCity;
    }

    public String getLocationCountryCode() {
        return locationCountryCode;
    }

    public UUID getProposalUserId() {
        return proposalUserId;
    }

    public LocalDateTime getProposalDate() {
        return proposalDate;
    }
}
