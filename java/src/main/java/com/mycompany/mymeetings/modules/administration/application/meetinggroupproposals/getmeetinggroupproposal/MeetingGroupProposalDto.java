package com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals.getmeetinggroupproposal;

import java.time.LocalDateTime;
import java.util.UUID;

public class MeetingGroupProposalDto {

    private UUID id;
    private String name;
    private String description;
    private String locationCity;
    private String locationCountryCode;
    private UUID proposalUserId;
    private LocalDateTime proposalDate;
    private String statusCode;
    private LocalDateTime decisionDate;
    private UUID decisionUserId;
    private String decisionCode;
    private String decisionRejectReason;

    public MeetingGroupProposalDto() {
    }

    public MeetingGroupProposalDto(UUID id, String name, String description, String locationCity,
                                    String locationCountryCode, UUID proposalUserId, LocalDateTime proposalDate,
                                    String statusCode, LocalDateTime decisionDate, UUID decisionUserId,
                                    String decisionCode, String decisionRejectReason) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.locationCity = locationCity;
        this.locationCountryCode = locationCountryCode;
        this.proposalUserId = proposalUserId;
        this.proposalDate = proposalDate;
        this.statusCode = statusCode;
        this.decisionDate = decisionDate;
        this.decisionUserId = decisionUserId;
        this.decisionCode = decisionCode;
        this.decisionRejectReason = decisionRejectReason;
    }

    public UUID getId() { return id; }
    public void setId(UUID id) { this.id = id; }
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }
    public String getLocationCity() { return locationCity; }
    public void setLocationCity(String locationCity) { this.locationCity = locationCity; }
    public String getLocationCountryCode() { return locationCountryCode; }
    public void setLocationCountryCode(String locationCountryCode) { this.locationCountryCode = locationCountryCode; }
    public UUID getProposalUserId() { return proposalUserId; }
    public void setProposalUserId(UUID proposalUserId) { this.proposalUserId = proposalUserId; }
    public LocalDateTime getProposalDate() { return proposalDate; }
    public void setProposalDate(LocalDateTime proposalDate) { this.proposalDate = proposalDate; }
    public String getStatusCode() { return statusCode; }
    public void setStatusCode(String statusCode) { this.statusCode = statusCode; }
    public LocalDateTime getDecisionDate() { return decisionDate; }
    public void setDecisionDate(LocalDateTime decisionDate) { this.decisionDate = decisionDate; }
    public UUID getDecisionUserId() { return decisionUserId; }
    public void setDecisionUserId(UUID decisionUserId) { this.decisionUserId = decisionUserId; }
    public String getDecisionCode() { return decisionCode; }
    public void setDecisionCode(String decisionCode) { this.decisionCode = decisionCode; }
    public String getDecisionRejectReason() { return decisionRejectReason; }
    public void setDecisionRejectReason(String decisionRejectReason) { this.decisionRejectReason = decisionRejectReason; }
}
