package com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals.getmeetinggroupproposals;

import com.mycompany.mymeetings.modules.administration.application.contracts.QueryHandler;
import com.mycompany.mymeetings.modules.administration.application.meetinggroupproposals.getmeetinggroupproposal.MeetingGroupProposalDto;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.List;
import java.util.UUID;

@Service
public class GetMeetingGroupProposalsQueryHandler
        implements QueryHandler<GetMeetingGroupProposalsQuery, List<MeetingGroupProposalDto>> {

    private final JdbcTemplate jdbcTemplate;

    public GetMeetingGroupProposalsQueryHandler(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    @Override
    public List<MeetingGroupProposalDto> handle(GetMeetingGroupProposalsQuery query) {
        String sql = """
                SELECT Id, Name, Description, LocationCity, LocationCountryCode,
                       ProposalUserId, ProposalDate, StatusCode,
                       DecisionDate, DecisionUserId, DecisionCode, DecisionRejectReason
                FROM administration.MeetingGroupProposals
                """;

        return jdbcTemplate.query(sql, (rs, rowNum) -> mapRow(rs));
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
