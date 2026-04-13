package com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals;

import com.mycompany.mymeetings.buildingblocks.domain.ValueObject;

public class MeetingGroupProposalStatus extends ValueObject {

    private final String value;

    private MeetingGroupProposalStatus(String value) {
        this.value = value;
    }

    public static MeetingGroupProposalStatus toVerify() {
        return new MeetingGroupProposalStatus("ToVerify");
    }

    public static MeetingGroupProposalStatus verified() {
        return new MeetingGroupProposalStatus("Verified");
    }

    public static MeetingGroupProposalStatus create(String value) {
        return new MeetingGroupProposalStatus(value);
    }

    public String getValue() {
        return value;
    }
}
