package com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals.getmeetinggroupproposal;

import com.mycompany.mymeetings.modules.administration.application.contracts.QueryHandler;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.UUID;

@Service
public class GetMeetingGroupProposalQueryHandler
        implements QueryHandler<GetMeetingGroupProposalQuery, MeetingGroupProposalDto> {

    private final JdbcTemplate jdbcTemplate;

    public GetMeetingGroupProposalQueryHandler(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    @Override
    public MeetingGroupProposalDto handle(GetMeetingGroupProposalQuery query) {
        String sql = """
                SELECT Id, Name, Description, LocationCity, LocationCountryCode,
                       ProposalUserId, ProposalDate, StatusCode,
                       DecisionDate, DecisionUserId, DecisionCode, DecisionRejectReason
                FROM administration.MeetingGroupProposals
                WHERE Id = ?
                """;

        return jdbcTemplate.queryForObject(sql,
                (rs, rowNum) -> mapRow(rs),
                query.getMeetingGroupProposalId().toString());
    }

    private MeetingGroupProposalDto mapRow(ResultSet rs) throws SQLException {
        MeetingGroupProposalDto dto = new MeetingGroupProposalDto();
        dto.setId(UUID.fromString(rs.getString("Id")));
        dto.setName(rs.getString("Name"));
        dto.setDescription(rs.getString("Description"));
        dto.setLocationCity(rs.getString("LocationCity"));
        dto.setLocationCountryCode(rs.getString("LocationCountryCode"));
        String proposalUserId = rs.getString("ProposalUserId");
        dto.setProposalUserId(proposalUserId != null ? UUID.fromString(proposalUserId) : null);
        dto.setProposalDate(rs.getTimestamp("ProposalDate") != null ? rs.getTimestamp("ProposalDate").toLocalDateTime() : null);
        dto.setStatusCode(rs.getString("StatusCode"));
        dto.setDecisionDate(rs.getTimestamp("DecisionDate") != null ? rs.getTimestamp("DecisionDate").toLocalDateTime() : null);
        String decisionUserId = rs.getString("DecisionUserId");
        dto.setDecisionUserId(decisionUserId != null ? UUID.fromString(decisionUserId) : null);
        dto.setDecisionCode(rs.getString("DecisionCode"));
        dto.setDecisionRejectReason(rs.getString("DecisionRejectReason"));
        return dto;
    }
}
