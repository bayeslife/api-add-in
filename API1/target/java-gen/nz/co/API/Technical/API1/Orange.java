
package nz.co.API.Technical.API1;

import javax.annotation.Generated;
import javax.validation.constraints.NotNull;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;
import org.apache.commons.lang3.builder.EqualsBuilder;
import org.apache.commons.lang3.builder.HashCodeBuilder;
import org.apache.commons.lang3.builder.ToStringBuilder;


/**
 * Orange
 * <p>
 * 
 * 
 */
@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "species"
})
public class Orange {

    /**
     * The type of orange
     * (Required)
     * 
     */
    @JsonProperty("species")
    @NotNull
    private String species;

    /**
     * The type of orange
     * (Required)
     * 
     * @return
     *     The species
     */
    @JsonProperty("species")
    public String getSpecies() {
        return species;
    }

    /**
     * The type of orange
     * (Required)
     * 
     * @param species
     *     The species
     */
    @JsonProperty("species")
    public void setSpecies(String species) {
        this.species = species;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }

    @Override
    public int hashCode() {
        return new HashCodeBuilder().append(species).toHashCode();
    }

    @Override
    public boolean equals(Object other) {
        if (other == this) {
            return true;
        }
        if ((other instanceof Orange) == false) {
            return false;
        }
        Orange rhs = ((Orange) other);
        return new EqualsBuilder().append(species, rhs.species).isEquals();
    }

}
