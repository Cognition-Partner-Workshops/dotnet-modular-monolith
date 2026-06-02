package com.mycompany.mymeetings.modules.administration.application.members.getmember;

import java.util.UUID;

public class MemberDto {

    private UUID id;
    private String login;
    private String email;
    private String firstName;
    private String lastName;
    private String name;

    public MemberDto() {
    }

    public MemberDto(UUID id, String login, String email, String firstName, String lastName, String name) {
        this.id = id;
        this.login = login;
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
        this.name = name;
    }

    public UUID getId() { return id; }
    public void setId(UUID id) { this.id = id; }
    public String getLogin() { return login; }
    public void setLogin(String login) { this.login = login; }
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    public String getFirstName() { return firstName; }
    public void setFirstName(String firstName) { this.firstName = firstName; }
    public String getLastName() { return lastName; }
    public void setLastName(String lastName) { this.lastName = lastName; }
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
}
