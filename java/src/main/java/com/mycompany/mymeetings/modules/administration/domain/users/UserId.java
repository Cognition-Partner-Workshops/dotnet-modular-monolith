package com.mycompany.mymeetings.modules.administration.domain.users;

import com.mycompany.mymeetings.buildingblocks.domain.TypedId;

import java.util.UUID;

public class UserId extends TypedId {

    public UserId(UUID value) {
        super(value);
    }
}
