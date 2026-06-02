package com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals.getmeetinggroupproposal;

import com.mycompany.mymeetings.modules.administration.application.contracts.QueryBase;

import java.util.UUID;

public class GetMeetingGroupProposalQuery extends QueryBase<MeetingGroupProposalDto> {

    private final UUID meetingGroupProposalId;

    public GetMeetingGroupProposalQuery(UUID meetingGroupProposalId) {
        this.meetingGroupProposalId = meetingGroupProposalId;
    }

    public UUID getMeetingGroupProposalId() {
        return meetingGroupProposalId;
    }
}
