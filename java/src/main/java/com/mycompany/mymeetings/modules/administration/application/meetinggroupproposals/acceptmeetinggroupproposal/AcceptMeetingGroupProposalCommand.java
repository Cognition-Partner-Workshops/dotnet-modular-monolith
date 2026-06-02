package com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals.acceptmeetinggroupproposal;

import com.mycompany.mymeetings.modules.administration.application.contracts.CommandBase;

import java.util.UUID;

public class AcceptMeetingGroupProposalCommand extends CommandBase {

    private final UUID meetingGroupProposalId;

    public AcceptMeetingGroupProposalCommand(UUID meetingGroupProposalId) {
        this.meetingGroupProposalId = meetingGroupProposalId;
    }

    public UUID getMeetingGroupProposalId() {
        return meetingGroupProposalId;
    }
}
