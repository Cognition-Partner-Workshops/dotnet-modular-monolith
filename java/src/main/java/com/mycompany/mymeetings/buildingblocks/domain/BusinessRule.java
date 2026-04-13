package com.mycompany.mymeetings.buildingblocks.domain;

public interface BusinessRule {

    boolean isBroken();

    String getMessage();
}
