package com.mycompany.mymeetings.modules.administration.application.members.getmember;

import com.mycompany.mymeetings.modules.administration.application.contracts.QueryHandler;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.UUID;

@Service
public class GetMemberQueryHandler implements QueryHandler<GetMemberQuery, MemberDto> {

    private final JdbcTemplate jdbcTemplate;

    public GetMemberQueryHandler(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    @Override
    public MemberDto handle(GetMemberQuery query) {
        String sql = """
                SELECT Id, Login, Email, FirstName, LastName, Name
                FROM administration.Members
                WHERE Id = ?
                """;

        return jdbcTemplate.queryForObject(sql, (rs, rowNum) -> {
            MemberDto dto = new MemberDto();
            dto.setId(UUID.fromString(rs.getString("Id")));
            dto.setLogin(rs.getString("Login"));
            dto.setEmail(rs.getString("Email"));
            dto.setFirstName(rs.getString("FirstName"));
            dto.setLastName(rs.getString("LastName"));
            dto.setName(rs.getString("Name"));
            return dto;
        }, query.getMemberId().toString());
    }
}
