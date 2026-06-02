package com.mycompany.mymeetings.modules.administration.domain.members;

import com.mycompany.mymeetings.buildingblocks.domain.TypedId;

import java.util.UUID;

public class MemberId extends TypedId {

    public MemberId(UUID value) {
        super(value);
    }
}
