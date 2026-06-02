package com.mycompany.mymeetings.modules.administration.domain.meetinggroupproposals;

import com.mycompany.mymeetings.buildingblocks.domain.ValueObject;
import jakarta.persistence.Column;
import jakarta.persistence.Embeddable;

@Embeddable
public class MeetingGroupLocation extends ValueObject {

    @Column(name = "LocationCity")
    private String city;

    @Column(name = "LocationCountryCode")
    private String countryCode;

    protected MeetingGroupLocation() {
    }

    private MeetingGroupLocation(String city, String countryCode) {
        this.city = city;
        this.countryCode = countryCode;
    }

    public static MeetingGroupLocation create(String city, String countryCode) {
        return new MeetingGroupLocation(city, countryCode);
    }

    public String getCity() {
        return city;
    }

    public String getCountryCode() {
        return countryCode;
    }
}
