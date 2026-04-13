package com.mycompany.mymeetings.modules.administration.application.members.getmember;

import com.mycompany.mymeetings.modules.administration.application.contracts.QueryBase;

import java.util.UUID;

public class GetMemberQuery extends QueryBase<MemberDto> {

    private final UUID memberId;

    public GetMemberQuery(UUID memberId) {
        this.memberId = memberId;
    }

    public UUID getMemberId() {
        return memberId;
    }
}
